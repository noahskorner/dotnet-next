import "../styles/globals.css";
import { AppProps } from "next/app";
import { NextPage } from "next";
import { ReactElement, ReactNode } from "react";
import GlobalLayout from "../layouts/global-layout/global-layout";

export type NextPageLayout = NextPage & {
  // eslint-disable-next-line no-unused-vars
  getLayout?: (page: ReactElement) => ReactNode;
};

export type AppPropsLayout = AppProps & {
  Component: NextPageLayout;
};

const App = ({ Component, pageProps }: AppPropsLayout) => {
  const getLayout = Component.getLayout ?? ((page) => page);

  return <GlobalLayout>{getLayout(<Component {...pageProps} />)}</GlobalLayout>;
};

export default App;
