import { hubMethods } from "../../commons/hubMethods";
import { storeActions } from "../../commons/storeActions";
import store from "../../store";

const sendMessage = (data) => {
  store.dispatch({
    type: storeActions.chat.invoke,
    method: hubMethods.invoke.SEND_MESSAGE,
    data,
  });
};

export const chatService = {
  sendMessage,
};
