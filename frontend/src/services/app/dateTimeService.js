import moment from "moment";
import { constants } from "../../commons/constants";

const convertToDate = (target) => {
  if (!target) return;
  return moment(target).toDate();
};

const convert = (target, format = constants.dateTimeFormat.DATE_TIME) => {
  if (!target) return null;
  return moment(target).format(format);
};

const convertRelative = (target) => {
  if (!target) return null;
  return moment(target).fromNow();
};

const getDate = (format = constants.dateTimeFormat.DATE) => {
  return moment().format(format);
};

const getYear = () => {
  return moment().format("YYYY");
};

export const dateTimeService = {
  convertToDate,
  convert,
  convertRelative,
  getDate,
  getYear,
};
