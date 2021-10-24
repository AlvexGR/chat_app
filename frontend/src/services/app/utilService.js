const replaceAll = (source, find, replace) => {
  if (!source) return "";
  return source.replace(new RegExp(find, "g"), replace);
};

export const utilService = {
  replaceAll,
};
