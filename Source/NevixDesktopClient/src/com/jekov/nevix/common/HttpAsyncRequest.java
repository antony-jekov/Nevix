package com.jekov.nevix.common;

import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpPut;
import org.apache.http.util.ByteArrayBuffer;

import java.io.*;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.ProtocolException;
import java.net.URL;
import java.util.concurrent.Callable;

/**
 * Created by Antony Jekov on 3/29/2014.
 */
public class HttpAsyncRequest implements Callable<String> {

    private final CallBack callBack;

    private String targetURL;
    private String sessionKey;
    private String urlParameters;
    private String requestType;

    public HttpAsyncRequest(CallBack callBack) {
        this.callBack = callBack;
    }

    public void prepareCall(String url, String requestType, String sessionKey, String requestBody) {
        this.targetURL = url;
        this.requestType = requestType;
        this.urlParameters = requestBody;
        this.sessionKey = sessionKey;
    }

    @Override
    public String call() throws Exception {
        Thread.sleep(5000);

        String result = "";
        HttpURLConnection httpCon = null;

        try {
            //Uses URL and HttpURLConnection for server asyncOperationsManager.
            URL url = new URL(targetURL);
            httpCon = (HttpURLConnection) url.openConnection();

            httpCon.setDoInput(true);
            httpCon.setDoOutput(true);
            httpCon.setUseCaches(false);
            httpCon.setChunkedStreamingMode(0);

            //properties of SOAPAction header
            httpCon.addRequestProperty("Content-Type", "application/json; charset=utf-8");
            httpCon.addRequestProperty("X-SessionKey", String.valueOf(sessionKey));

            if (!requestType.equals(AsyncOperationsManager.GET)) {
                httpCon.addRequestProperty("Content-Length", String.valueOf(urlParameters.length()));
                if (requestType.equals(AsyncOperationsManager.PUT))
                    httpCon.setRequestMethod(HttpPut.METHOD_NAME);
                else if (requestType.equals(AsyncOperationsManager.POST))
                    httpCon.setRequestMethod(HttpPost.METHOD_NAME);
            } else
                httpCon.setRequestMethod(AsyncOperationsManager.GET);

            //sending request to the server.
            OutputStream outputStream = httpCon.getOutputStream();
            Writer writer = new OutputStreamWriter(outputStream);
            writer.write(urlParameters);
            writer.flush();
            writer.close();


            //getting the response from the server
            InputStream inputStream = httpCon.getInputStream();
            BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(inputStream));
            ByteArrayBuffer byteArrayBuffer = new ByteArrayBuffer(50);

            int intResponse = httpCon.getResponseCode();

            while ((intResponse = bufferedReader.read()) != -1) {
                byteArrayBuffer.append(intResponse);
            }

            result = new String(byteArrayBuffer.toByteArray());
        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            if (httpCon != null)
                httpCon.disconnect();
        }

        callBack.onResult(result);
        return null;
    }
}
