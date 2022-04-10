import { callApi } from './service'

export const accountService = {
    register,
    confirmAccount,
    forgotPassword,
    resetPassword,
    login,
    logout
};

async function register(model) {
    return callApi('/account/register', 'POST', model)
        .then(result => result);
}

async function confirmAccount(model) {
    return callApi('/account/confirmaccount', 'POST', model)
        .then(result => result);
}

async function forgotPassword(model) {
    return callApi('/account/forgotpassword', 'POST', model)
        .then(result => result);
}

async function resetPassword(model) {
    return callApi('/account/resetPassword', 'POST', model)
        .then(result => result);
}

async function login(model) {
    return callApi('/account/login', 'POST', model)
        .then(result => {
            if (result.success)
                sessionStorage.setItem('tm_access', result.data.token);

            return result;
        });
}

async function logout() {
    sessionStorage.removeItem('rp_token');
    sessionStorage.removeItem('rp_company');
    window.location.href = "/";
}


