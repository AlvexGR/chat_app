import { storeActions } from "./storeActions";

const invoke = {
  SEND_MESSAGE: "SendMessage",
};

const listen = {
  RECEIVE_MESSAGE: "ReceiveMessage",
};

const listeners = [
  {
    method: listen.RECEIVE_MESSAGE,
    store: storeActions.chat.receive,
  },
];

export const hubMethods = {
  invoke,
  listen,
  listeners,
};
