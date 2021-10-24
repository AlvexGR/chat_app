import { constants } from "../../commons/constants";
import { localStorageService } from "../app/localStorageService";
import { apiService } from "./apiService";

const API_VERSION = process.env.API_VERSION;
const BASE_URL = "api/auth";

const register = async (data) => {
  return await apiService.httpPost(`${BASE_URL}/${API_VERSION}/register`, data);
}

const login = async (username, password) => {
  return await apiService.httpPost(`${BASE_URL}/${API_VERSION}/login`, { username, password });
};

const logout = () => {
  localStorageService.removeKey(constants.storeKeys.LOGIN_USER);
  localStorageService.removeKey(constants.storeKeys.ACCESS_TOKEN);
};

const isLoggedIn = () => {
  return (
    localStorageService.getValue(constants.storeKeys.LOGIN_USER) &&
    localStorageService.getValue(constants.storeKeys.ACCESS_TOKEN)
  );
};

const isAdmin = () => {
  const userRaw = localStorageService.getValue(constants.storeKeys.LOGIN_USER);
  if (!userRaw) return false;
  const user = JSON.parse(userRaw);
  return user.role === constants.roles.ADMIN;
};

const getToken = () => {
  return localStorageService.getValue(constants.storeKeys.ACCESS_TOKEN);
};

const getName = () => {
  const userRaw = localStorageService.getValue(constants.storeKeys.LOGIN_USER);
  if (!userRaw) return "";
  const user = JSON.parse(userRaw);
  return user.name;
}

const getEmail = () => {
  const userRaw = localStorageService.getValue(constants.storeKeys.LOGIN_USER);
  if (!userRaw) return "";
  const user = JSON.parse(userRaw);
  return user.email;
}

const getId = () => {
  const userRaw = localStorageService.getValue(constants.storeKeys.LOGIN_USER);
  if (!userRaw) return "";
  const user = JSON.parse(userRaw);
  return user._id;
}

export const authService = {
  register,
  login,
  logout,
  getToken,
  isLoggedIn,
  getName,
  getEmail,
  getId,
  isAdmin
};
