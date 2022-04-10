import { callApi } from './service'

export const catalogService = {
    getAll
};

async function getAll(model) {
    return callApi('/catalog/countries', 'GET')
        .then(result => result)
        .catch(error => {
            console.log(error);
            return [];
        });
}



