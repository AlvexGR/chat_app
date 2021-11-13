import { apiService } from "./apiService";

const API_VERSION = process.env.REACT_APP_API_VERSION;
const BASE_URL = "api/users";

const changePassword = async (currentPassword, newPassword) => {
  return await apiService.httpPut(`${BASE_URL}/${API_VERSION}/change-password`, { currentPassword, newPassword });
};

const confirmAccount = async (token) => {
  return await apiService.httpPost(`${BASE_URL}/${API_VERSION}/confirm-account/${token}`, {});
}

const sendConfirmation = async() => {
  return await apiService.httpPost(`${BASE_URL}/${API_VERSION}/send-confirmation`);
}

export const userService = {
  changePassword,
  confirmAccount,
  sendConfirmation
};
