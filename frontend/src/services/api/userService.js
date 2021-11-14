import { apiService } from "./apiService";

const API_VERSION = process.env.REACT_APP_API_VERSION;
const BASE_URL = `api/${API_VERSION}/users`;

const changePassword = async (currentPassword, newPassword) => {
  return await apiService.httpPut(`${BASE_URL}/change-password`, { currentPassword, newPassword });
};

const confirmAccount = async (token) => {
  return await apiService.httpPost(`${BASE_URL}/confirm-account/${token}`, {});
}

const sendConfirmation = async() => {
  return await apiService.httpPost(`${BASE_URL}/send-confirmation`);
}

export const userService = {
  changePassword,
  confirmAccount,
  sendConfirmation
};
