var API_PATH = "http://localhost:58548/api/";

var axiosInstance = axios.create({
    baseURL: API_PATH,
    responseType: 'json',    
});
