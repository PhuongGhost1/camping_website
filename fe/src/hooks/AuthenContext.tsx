import {
  createContext,
  ReactNode,
  useContext,
  useEffect,
  useState,
} from "react";
import { ApiGateway } from "../services/api/ApiService";
import { UserProps } from "../App";

interface AuthenContextProps {
  login: (email: string, password: string) => Promise<boolean>;
  logout: () => Promise<void>;
  user: UserProps | null;
  setUser: React.Dispatch<React.SetStateAction<UserProps | null>>;
}

export const AuthenContext = createContext<AuthenContextProps | undefined>(
  undefined
);

const AuthenProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<UserProps | null>(null);

  const login = async (email: string, password: string): Promise<boolean> => {
    const success = await ApiGateway.LoginDefault(email, password);
    if (success) {
      await fetchUser();
    }
    return success;
  };

  const logout = async (): Promise<void> => {
    await ApiGateway.LogOut();
    setUser(null);
  };

  const fetchUser = async () => {
    try {
      const token = localStorage.getItem("accessToken");
      if (!token) setUser(null);

      const data = await ApiGateway.getUser<UserProps>();
      setUser(data);
    } catch {
      setUser(null);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("accessToken");
    if (token) {
      fetchUser();
    }
  }, []);

  return (
    <AuthenContext.Provider value={{ login, logout, user, setUser }}>
      {children}
    </AuthenContext.Provider>
  );
};

export default AuthenProvider;

export const useAuthenContext = () => {
  const context = useContext(AuthenContext);
  if (!context) {
    throw new Error("useAuthenContext must be used within an AuthenProvider");
  }
  return context;
};
