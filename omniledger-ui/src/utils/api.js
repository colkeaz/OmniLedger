const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:8080';

export const apiFetch = async (endpoint, options = {}) => {
  const url = `${API_BASE_URL}${endpoint}`;
  
  const defaultOptions = {
    headers: {
      'Content-Type': 'application/json; charset=utf-8',
    },
  };

  const mergedOptions = {
    ...defaultOptions,
    ...options,
    headers: {
      ...defaultOptions.headers,
      ...options.headers,
    },
  };

  const response = await fetch(url, mergedOptions);
  
  if (!response.ok && response.status !== 401 && response.status !== 400) {
    throw new Error(`API error: ${response.statusText}`);
  }
  
  return response;
};

export const getApiUrl = (endpoint) => `${API_BASE_URL}${endpoint}`;
