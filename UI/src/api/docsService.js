import { apiFetchBlob,apiFetchJson, urlBase } from "./apiClient";

export function getDocs(){
    return apiFetchJson('docs',{
        method : 'GET'
    })
}
export function getDoc(id){
    return (`${urlBase}/docs/download/${id}`)
}

export function addDoc(data){
     return apiFetchBlob('docs',{
        method : 'POST',
        body : data
    })
}