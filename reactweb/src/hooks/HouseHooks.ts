import axios, { AxiosError } from "axios";
import { useQuery } from "react-query";
import config from "../config";
import { House } from "../types/house";


const useFetchHouses = () =>
{
    return useQuery<House[], AxiosError>("houses", () =>
    
        axios.get(`${config.baseApiUrl}/houses`).then(
            (resp) => resp.data)
        );
}

const useFetchHouse = (id: number) =>
{
    return useQuery<House, AxiosError>(["houses", id],() =>
        axios.get(`${config.baseApiUrl}/houses/${id}`).then(
            (resp) => resp.data)
        );
}


export default useFetchHouses;
export {useFetchHouse};