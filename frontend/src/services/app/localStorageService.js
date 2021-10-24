const setValue = (key, value) => {
  if (!key) return;
  localStorage.setItem(key, value);
}

const getValue = (key) => {
  return localStorage.getItem(key);
}

const removeKey = (key) => {
  if (!key) return;
  localStorage.removeItem(key);
}

export const localStorageService = {
  setValue,
  getValue,
  removeKey
}
