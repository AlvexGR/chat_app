import axios from "axios";
import { constants } from "../../commons/constants";
import { messages } from "../../commons/messages";
import { storeActions } from "../../commons/storeActions";
import store from "../../store";
import { authService } from "./authService";

const PROTOCOL = process.env.REACT_APP_PROTOCOL;
const BASE_URL = process.env.REACT_APP_API_URL;

axios.interceptors.response.use(
  (res) => {
    if (!res.data) {
      throw new Error("Response data is null");
    }
    if (res.data.success) return res.data;
    store.dispatch({
      type: storeActions.toaster.set,
      ...{
        toasterType: messages.toasterTypes.ERROR,
        message: messages.errorMessages[res.data.errorCode],
      },
    });
    return res.data;
  },
  (error) => {
    let message =
      messages.errorMessages[
        messages.errorCodes.INTERNAL_SERVER_ERROR
      ];
    if (error.response && error.response.status) {
      switch (error.response.status) {
        case 401:
          authService.logout();
          message =
            messages.errorMessages[messages.errorCodes.UNAUTHORIZED];
          store.dispatch({
            type: storeActions.auth.set,
            ...{
              authorized: false,
            },
          });
          break;
        case 403:
          message =
            messages.errorMessages[messages.errorCodes.FORBIDDEN];
          store.dispatch({
            type: storeActions.auth.set,
            ...{
              allowAccess: false,
            },
          });
          break;
        default:
          break;
      }
    }

    store.dispatch({
      type: storeActions.toaster.set,
      ...{
        toasterType: messages.toasterTypes.ERROR,
        message,
      },
    });
    return Promise.reject(error);
  }
);


const getHeaders = (extra) => {
  const token = authService.getToken();
  return token
    ? {
        Authorization: `${constants.authSchema} ${token}`,
        ...extra,
      }
    : "";
};

const httpGet = async (url) => {
  try {
    return await axios.get(`${BASE_URL}/${url}`, {
      headers: getHeaders(),
    });
  } catch (error) {
    console.log(error);
    return null;
  }
};

const httpPost = async (url, data) => {
  try {
    return await axios.post(`${PROTOCOL}://${BASE_URL}/${url}`, data, {
      headers: getHeaders(),
    });
  } catch (error) {
    console.log(error);
    return null;
  }
};

const httpPut = async (url, data) => {
  try {
    return await axios.put(`${BASE_URL}/${url}`, data, {
      headers: getHeaders(),
    });
  } catch (error) {
    console.log(error);
    return null;
  }
};

const httpDelete = async (url) => {
  try {
    return await axios.delete(`${BASE_URL}/${url}`, {
      headers: getHeaders(),
    });
  } catch (error) {
    console.log(error);
    return null;
  }
};

const httpPostForm = async (url, data) => {
  try {
    return await axios.post(`${BASE_URL}/${url}`, data, {
      headers: getHeaders({
        "Content-Type": "multipart/form-data",
      }),
    });
  } catch (error) {
    console.log(error);
    return null;
  }
};

const httpPutForm = async (url, data) => {
  try {
    return await axios.put(`${BASE_URL}/${url}`, data, {
      headers: getHeaders({
        "Content-Type": "multipart/form-data",
      }),
    });
  } catch (error) {
    console.log(error);
    return null;
  }
};

const fetchUrl = async (url, extra) => {
  return await fetch(url, extra);
};

const delay = (ms) => {
  return new Promise((resolve) => setTimeout(resolve, ms));
};

const getFormData = (data) => {
  const formData = new FormData();
  Object.keys(data).forEach((key) => formData.append(key, data[key]));
  return formData;
};

export const apiService = {
  httpGet,
  httpPost,
  httpPut,
  httpDelete,
  httpPostForm,
  httpPutForm,
  delay,
  getFormData,
  fetchUrl,
};
