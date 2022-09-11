import { useContext } from "react";
import { WindowContext } from "./window-context";

const useWindowSize = () => {
  return useContext(WindowContext);
};

export default useWindowSize;
