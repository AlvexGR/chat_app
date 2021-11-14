import { useEffect, useState } from "react";
import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { hubMethods } from "../../commons/hubMethods";
import { authService } from "../../services/api/authService";

const BASE_URL = process.env.REACT_APP_API_URL;
const PROTOCOL = process.env.REACT_APP_PROTOCOL;
const HUB_VERSION = process.env.REACT_APP_HUB_VERSION;

const Chat = () => {
  const [message, setMessage] = useState("");
  const [connected, setConnected] = useState(false);
  const [chatHub, setChatHub] = useState(null);
  const [toSendMessage, setToSendMessage] = useState("");

  // TODO: Refactor to Layout and use props
  const initConnection = async () => {
    const connection = new HubConnectionBuilder()
      .withUrl(`${PROTOCOL}://${BASE_URL}/${HUB_VERSION}/chat`, {
        accessTokenFactory: () => authService.getToken(),
      })
      .build();

    connection.onclose(() => setConnected(false));
    await startConnection(connection);
  };

  const startConnection = async (connection) => {
    try {
      await connection.start();
      setConnected(true);
      setChatHub(connection);
      listenToHub(connection);
    } catch (err) {
      console.log(err);
      setTimeout(() => startConnection(connection), 5000);
    }
  };

  useEffect(() => {
    if (connected) return;
    initConnection();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [connected]);

  const listenToHub = (connection) => {
    connection.on(hubMethods.listen.RECEIVE_MESSAGE, (user, message) => {
      setMessage((prev) => {
        if (!prev) return `${user}: ${message}`;
        return `${prev}\n${user}: ${message}`;
      });
    });
  };

  const sendMessage = async () => {
    if (
      !connected ||
      !chatHub ||
      chatHub.state !== HubConnectionState.Connected ||
      !toSendMessage
    )
      return;
    try {
      await chatHub.invoke(
        hubMethods.invoke.SEND_MESSAGE,
        "Test",
        toSendMessage
      );
      setToSendMessage("");
    } catch (err) {
      console.log(err);
    }
  };

  const onPress = async (e) => {
    if (e.charCode !== 13) return;
    e.preventDefault();
    await sendMessage();
  };

  return (
    <div className="row vh-100">
      <div className="col-lg-12">
        <div className="pre-line">{message}</div>
        <textarea
          className="form-control"
          rows="5"
          value={toSendMessage}
          onChange={(e) => setToSendMessage(e.target.value.trim())}
          onKeyPress={onPress}
        ></textarea>
        <button className="btn btn-primary" onClick={sendMessage}>
          <i className="fas fa-paper-plane"></i>
        </button>
      </div>
    </div>
  );
};

export default Chat;
