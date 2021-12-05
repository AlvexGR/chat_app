import { useEffect, useState } from "react";
import MainContent from "./MainContent";
import SideBar from "./SideBar";
import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { authService } from "../services/api/authService";
import { hubMethods } from "../commons/hubMethods";
import { connect, useDispatch } from "react-redux";
import { messages } from "../commons/messages";
import { constants } from "../commons/constants";
import { useHistory } from "react-router";

const BASE_URL = process.env.REACT_APP_API_URL;
const PROTOCOL = process.env.REACT_APP_PROTOCOL;
const HUB_VERSION = process.env.REACT_APP_HUB_VERSION;

const Layout = (props) => {
  const dispatch = useDispatch();
  const history = useHistory();
  const [chatHub, setChatHub] = useState(null);

  useEffect(() => {
    initConnection();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    invoke(props.hubInvokeReducer.method, props.hubInvokeReducer.data);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.hubInvokeReducer]);

  const initConnection = async () => {
    if (chatHub) {
      chatHub.stop();
    }

    const connection = new HubConnectionBuilder()
      .withUrl(`${PROTOCOL}://${BASE_URL}/${HUB_VERSION}/chat`, {
        accessTokenFactory: () => authService.getToken(),
      })
      .build();

    connection.onclose((err) => {
      if (err.message.endsWith(messages.errorCodes.UNAUTHORIZED)) {
        authService.logout();
        history.push(constants.routing.LOGIN);
        return;
      }
      setTimeout(() => initConnection(), 10000);
    });
    await startConnection(connection);
  };

  const startConnection = async (connection) => {
    try {
      if (connection.state !== HubConnectionState.Disconnected) return;
      await connection.start();
      setChatHub(connection);
      listenToHub(connection);
    } catch {
      setTimeout(() => initConnection(), 10000);
    }
  };

  const checkHubState = () => {
    return chatHub && chatHub.state === HubConnectionState.Connected;
  };

  const listenToHub = (connection) => {
    if (!connection) return;
    for (let i = 0; i < hubMethods.listeners.length; i++) {
      connection.on(hubMethods.listeners[i].method, (data) =>
        dispatch({
          type: hubMethods.listeners[i].store,
          data,
        })
      );
    }
  };

  const invoke = async (method, data) => {
    if (!checkHubState() || !method) return;
    await chatHub.invoke(method, data);
  };

  return (
    <div className="row">
      <SideBar />
      <MainContent />
    </div>
  );
};

const mapStateToProps = (state) => {
  return { hubInvokeReducer: state.hubInvokeReducer };
};

export default connect(mapStateToProps)(Layout);
