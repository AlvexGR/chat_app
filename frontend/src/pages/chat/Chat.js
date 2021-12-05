import { useEffect, useState } from "react";
import { connect } from "react-redux";
import { chatService } from "../../services/hub/chatService";

const Chat = (props) => {
  const [message, setMessage] = useState("");
  const [toSendMessage, setToSendMessage] = useState("");

  useEffect(() => {
    if (!props.hubListenReducer.data) return;
    setMessage((prev) => {
      const data = props.hubListenReducer.data;
      const newMessageLine = `${data.receiverId}: ${data.content}`;
      if (!prev) return newMessageLine;
      return `${prev}\n${newMessageLine}`;
    });
  }, [props.hubListenReducer]);

  const sendMessage = () => {
    try {
      chatService.sendMessage({
        receiverId: "618f247f6a5526698ffd986d",
        content: toSendMessage,
      });
      setToSendMessage("");
    } catch (err) {
      console.log(err);
    }
  };

  const onPress = (e) => {
    if (e.charCode !== 13) return;
    e.preventDefault();
    sendMessage();
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

const mapStateToProps = (state) => {
  return {
    hubListenReducer: state.hubListenReducer,
  };
};

export default connect(mapStateToProps)(Chat);
