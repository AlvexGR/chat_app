const authSchema = "Bearer";

const storeKeys = {
  LOGIN_USER: "LOGIN_USER",
  ACCESS_TOKEN: "ACCESS_TOKEN",
};

const dateTimeFormat = {
  DATE: "MM-DD-YYYY",
  DATE_2: "YYYY-MM-DD",
  DATE_3: "YYYYMMDD",
  DATE_4: "DD-MM-YYYY",
  DATE_5: "MMM-YYYY",
  DATE_6: "MMM Do YY",
  DATE_TIME: "MM-DD-YYYY HH:mm",
  DATE_TIME_2: "MM-DD-YYYY HH:mm:ss",
  DATE_TIME_3: "DD-MM-YYYY HH:mm",
  DATE_TIME_4: "YYYY-MM-DDTHH:mm",
  DATE_TIME_5: "HH:mm MM-DD-YYYY",
  DATE_TIME_6: "YYYY-MM-DD HH:mm",
  DATE_TIME_7: "MMM Do YYYY, HH:mm",
};

const routing = {
  HOME: "/",
  LOGIN: "/login",
  GOOGLE_LOGIN_REDIRECT: "/google-login-redirect",
  REGISTER: "/register",
  FORGOT_PASSWORD: "/forgot-password",
  ADMIN: "/admin",
  ADMINS: {
    USERS: "/users"
  },
  FORBIDDEN: "/forbidden",
  NOT_FOUND: "*",
};

const roles = {
  USER: 1,
  ADMIN: 2,
};

const regexPatterns = {
  PASSWORD: /(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{7,})/,
  EMAIL: /\S+@\S+\.\S+/,
};

export const constants = {
  authSchema,
  dateTimeFormat,
  storeKeys,
  routing,
  roles,
  regexPatterns
}