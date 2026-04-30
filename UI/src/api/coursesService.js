import { apiFetchJson } from './apiClient.js';


export async function getCourses(){
    return apiFetchJson('courses',{
        method : 'GET'
    })
}
export async function getCourse(Id){
    return apiFetchJson(`courses/${Id}`,{
        method : 'GET'
    })
}
export async function addCourse(data){
    return apiFetchJson(`courses`,{
        method : 'POST',
        body : JSON.stringify(data)
    })
}
export async function changeCourseState(Id){
    return apiFetchJson(`courses/${Id}`,{
        method : 'PATCH',
    })
}