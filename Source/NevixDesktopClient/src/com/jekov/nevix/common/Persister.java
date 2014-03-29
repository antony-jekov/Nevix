package com.jekov.nevix.common;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

/**
 * Created by Antony Jekov on 3/28/2014.
 */
public class Persister {
    private static final String ROOT_ADDRESS = "http://nevix.apphb.com/api/";

    private static Persister instance = new Persister();

    public static Persister instance() {
        return instance;
    }

    AsyncOperationsManager asyncOperationsManager;

    public Persister() {
        this.asyncOperationsManager = new AsyncOperationsManager();
    }

    private void getRequest(String address, String sessionKey, final CallBack callBack) {
        asyncOperationsManager.performHttpGet(address, sessionKey, callBack);
    }

    private void postRequest(String url, String sessionKey, String requestBody, CallBack callBack) {
        this.asyncOperationsManager.performHttpPost(url, sessionKey, requestBody, callBack);
    }

    private void putRequest(CallBack callBack) {

    }

    public String excutePost(String targetURL, String urlParameters)
    {
        URL url;
        HttpURLConnection connection = null;
        try {
            //Create asyncOperationsManager
            url = new URL(targetURL);
            connection = (HttpURLConnection)url.openConnection();
            connection.setRequestMethod("POST");
            connection.setRequestProperty("Content-Type", "application/json");

            connection.setRequestProperty("Content-Length", "" +
                    Integer.toString(urlParameters.getBytes().length));
            connection.setRequestProperty("Content-Language", "en-US");

            connection.setUseCaches (false);
            connection.setDoInput(true);
            connection.setDoOutput(true);

            //Send request
            DataOutputStream wr = new DataOutputStream(
                    connection.getOutputStream ());
            wr.writeBytes (urlParameters);
            wr.flush ();
            wr.close ();

            //Get Response
            InputStream is = connection.getInputStream();
            BufferedReader rd = new BufferedReader(new InputStreamReader(is));
            String line;
            StringBuffer response = new StringBuffer();
            while((line = rd.readLine()) != null) {
                response.append(line);
                response.append('\r');
            }
            rd.close();
            return response.toString();

        } catch (Exception e) {

            e.printStackTrace();
            return null;

        } finally {

            if(connection != null) {
                connection.disconnect();
            }
        }
    }
}
