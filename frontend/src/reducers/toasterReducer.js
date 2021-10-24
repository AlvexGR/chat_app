import { storeActions } from "../commons/storeActions";

const initialState = {
  toasterType: "",
  message: "",
  counter: 0,
};

export const toasterReducer = (state = initialState, { type, ...data }) => {
  switch (type) {
    case storeActions.toaster.set:
      return {
        ...state,
        ...data,
        counter: state.counter + 1,
      };
    default:
      return state;
  }
};
