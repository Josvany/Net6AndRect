import axios, { AxiosError, AxiosResponse } from "axios";
import { useMutation, useQuery, useQueryClient } from "react-query";
import config from "../config";
import { House } from "../types/house";
import { useNavigate } from 'react-router-dom';


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


const useAddHouse = () =>
{
    const nav = useNavigate();
    const queryClient = useQueryClient();

    return useMutation<AxiosResponse, AxiosError, House>(
        (h) => axios.post(`${config.baseApiUrl}/houses`, h),
        {
            onSuccess: () => {
                queryClient.invalidateQueries("houses");
                nav("/");
            } 
        }
    );
}

const useUpdateHouse = () =>
{
    const nav = useNavigate();
    const queryClient = useQueryClient();

    return useMutation<AxiosResponse, AxiosError, House>(
        (h) => axios.put(`${config.baseApiUrl}/houses`, h),
        {
            onSuccess: (_, house) => {
                queryClient.invalidateQueries("houses");
                nav(`/house/${house.id}`);
            } 
        }
    );
}

const useDeleteHouse = () =>
{
    const nav = useNavigate();
    const queryClient = useQueryClient();

    return useMutation<AxiosResponse, AxiosError, House>(
        (h) => axios.delete(`${config.baseApiUrl}/houses/${h.id}`),
        {
            onSuccess: () => {
                queryClient.invalidateQueries("house");
                nav("/");
            } 
        }
    );
}

export default useFetchHouses;
export {useFetchHouse, useAddHouse, useUpdateHouse, useDeleteHouse};