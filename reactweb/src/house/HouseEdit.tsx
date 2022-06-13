import { useParams } from "react-router-dom";
import ApiStatus from "../apiStatus";
import { useFetchHouse, UpdateHouse } from "../hooks/HouseHooks";
import HouseForm from "./HouseForm";

const HouseEdit = () => {
  const { id } = useParams();
  if (!id) throw Error("Need a house id");
  const houseId = parseInt(id);

  const { data, status, isSuccess } = useFetchHouse(houseId);
  const updateHouseMutation = UpdateHouse();

  if (!isSuccess) return <ApiStatus status={status} />;

  return (
    <>
      
      <HouseForm
        house={data}
        submitted={(house) => {
          updateHouseMutation.mutate(house);
        }}
      />
    </>
  );
};

export default HouseEdit;
