import { storeActions } from "../commons/storeActions";

const initialState = {
  authorized: false,
  isAdmin: false,
  allowAccess: true,
  counter: 0,
};

export const authReducer = (
  state = initialState,
  { type, skipCounter, ...data }
) => {
  switch (type) {
    case storeActions.auth.set:
      const counter = !skipCounter ? state.counter + 1 : state.counter;
      return {
        ...state,
        ...data,
        counter,
      };
    default:
      return state;
  }
};
