import { storeActions } from "../commons/storeActions";

const initialState = {
  method: "",
  data: {},
};

export const hubInvokeReducer = (
  state = initialState,
  { type, method, data }
) => {
  switch (type) {
    case storeActions.chat.invoke:
      return {
        ...state,
        method,
        data,
      };
    default:
      return state;
  }
};
