import axios from 'axios';

const API = axios.create({
  baseURL: 'http://localhost:5000',  // adjust if needed
  withCredentials: true, // needed for cookie auth
});

export default API;
