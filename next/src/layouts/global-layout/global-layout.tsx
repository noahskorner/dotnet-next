import Head from "next/head";
import { ReactNode } from "react";
import useWindowSize from "../../hooks/use-window-size";
import { WindowProvider } from "../../hooks/use-window-size/window-context";

interface GlobalLayoutProps {
  children: ReactNode;
}

const Layout = ({ children }: GlobalLayoutProps) => {
  const { heightStyle, widthStyle } = useWindowSize();
  return (
    <div style={{ width: widthStyle, height: heightStyle }}>{children}</div>
  );
};

const GlobalLayout = (props: GlobalLayoutProps) => {
  return (
    <WindowProvider>
      <>
        <Head>
          <meta
            name="viewport"
            content="width=device-width, initial-scale=1, maximum-scale=1"
          />
        </Head>
        <Layout {...props} />
      </>
    </WindowProvider>
  );
};

export default GlobalLayout;
