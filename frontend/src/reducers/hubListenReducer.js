import { storeActions } from "../commons/storeActions";

const initialState = {
  data: undefined,
};

export const hubListenReducer = (state = initialState, { type, data }) => {
  switch (type) {
    case storeActions.chat.receive:
      return {
        ...state,
        data,
      };
    default:
      return state;
  }
};
