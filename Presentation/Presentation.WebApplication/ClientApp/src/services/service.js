//import store from '../store/configureStore'
//import { toggleBlocking } from '../store/actions'

/**
 * Communication with the API
 * @param {*} url
 * @param {*} method
 * @param {*} body
 * @param {*} contentType
 */

export async function callApi(url, method, body, contentType = 'application/json') {
    var options = {
        method: method,
        headers: {
            'Content-Type': contentType,
            'Authorization': 'Bearer ' + sessionStorage["tm_access"]
        },
        body: JSON.stringify(body)
    };

    //store.dispatch(toggleBlocking())
    return fetch(`${process.env.REACT_APP_API_URL}${url}`, options)
        .then(response => handleResponse(response))
        .catch(handleError);
}

/**
 * Communication with the API file
 * @param {*} url
 * @param {*} method
 * @param {*} body
 */

export async function callApiFile(url, method, body) {
    var options = {
        method: method,
        headers: {
            'Authorization': 'Bearer ' + sessionStorage["tm_access"]
        },
        body: body
    };

    //store.dispatch(toggleBlocking())
    return fetch(`${process.env.REACT_APP_API_URL}${url}`, options)
        .then(response => handleResponse(response))
        .catch(handleError);
}

export function handleResponse(response) {
    //store.dispatch(toggleBlocking())
    if (!response.ok) {
        if (response.status === 401 || response.status === 403) {
            window.location.href = "/";
        }
        return Promise.reject("error");
    }
    const contentType = response.headers.get("content-type");
    if (contentType.includes("application/json;"))
        return response.text()
            .then(text => {
                let data

                try {
                    data = text && JSON.parse(text);
                }
                catch (e) {
                    data = text
                }
                return data;
            });
    else return response.blob();
}

export function handleError(error) {
    //store.dispatch(toggleBlocking())
    console.log('error', error)
    if (error.message === "Failed to fetch")
        error.message = "El Servicio no se encuentra disponible";
    return Promise.reject(error);
}