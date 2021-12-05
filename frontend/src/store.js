import { combineReducers, createStore } from "redux";
import { toasterReducer } from "./reducers/toasterReducer";
import { authReducer } from "./reducers/authReducer";
import { hubInvokeReducer } from "./reducers/hubInvokeReducer";
import { hubListenReducer } from "./reducers/hubListenReducer";

const store = createStore(
  combineReducers({
    toasterReducer,
    authReducer,
    hubInvokeReducer,
    hubListenReducer,
  }),
  window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__()
);
export default store;
