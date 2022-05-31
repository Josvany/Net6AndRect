import { useEffect, useState } from "react";
import config from "../config";
import { House } from "../types/house";


const useFetchHouses = () =>
{
    const [houses, setHouses] = useState<House[]>([]);
    
    useEffect(() => 
    {
        const fetchHouses = async () => 
        {
            const rps = await fetch(`${config.baseApiUrl}/houses`);
            const houses = await rps.json();
            setHouses(houses);
        }
        fetchHouses();

    }, []);

    return houses;
}

export default useFetchHouses;