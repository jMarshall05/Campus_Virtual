import { apiFetchBlob,apiFetchJson, urlBase } from "./apiClient";

export async function getDocs(){
    return apiFetchJson('docs',{
        method : 'GET'
    })
}
export async function getDoc(id){
    return (`${urlBase}/docs/download/${id}`)
}

export async function addDoc(data){
     return apiFetchBlob('docs',{
        method : 'POST',
        body : data
    })
}
export async function editDoc(id,data){
     return apiFetchBlob(`docs/${id}`,{
        method : 'PUT',
        body : data
    })
}

export async function deleteDoc(id){
    return apiFetchJson(`docs/${id}`,{
        method : 'DELETE'
    })
}