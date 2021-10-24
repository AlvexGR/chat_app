import { combineReducers, createStore } from "redux";
import { toasterReducer } from "./reducers/toasterReducer";
import { authReducer } from "./reducers/authReducer";

const store = createStore(
  combineReducers({
    toasterReducer,
    authReducer,
  }),
  window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__()
);
export default store;
