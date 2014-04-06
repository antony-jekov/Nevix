package com.antonyjekov.nevix.common;

import android.os.AsyncTask;

import com.antonyjekov.nevix.common.contracts.FailCallback;

import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpPut;
import org.apache.http.util.ByteArrayBuffer;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.Writer;
import java.net.HttpURLConnection;
import java.net.URL;

public class HttpAsyncRequest extends AsyncTask<Void, Void, String> {

    private boolean transactionSucceeded = true;

    private final OnResultCallBack callBack;
    private final FailCallback error;

    public static interface OnResultCallBack {
        void onResult(String result);
    }

    private HttpRequestType requestType;
    private String sessionKey;
    private String requestAddress;
    private String requestBody;

    public HttpAsyncRequest(OnResultCallBack callBack, FailCallback error) {
        this.callBack = callBack;
        this.error = error;
    }

    @Override
    protected String doInBackground(Void... strings) {
        String responseStorage; //storage of the response

        try {
            //Uses URL and HttpURLConnection for server connection.
            URL targetURL = new URL(requestAddress);
            HttpURLConnection httpCon = (HttpURLConnection) targetURL.openConnection();

            httpCon.setDoInput(true);
            httpCon.setUseCaches(false);
            httpCon.setChunkedStreamingMode(0);

            //properties of SOAPAction header
            httpCon.addRequestProperty("Content-Type", "application/json; charset=utf-8");
            httpCon.addRequestProperty(PersistentManager.SESSION_KEY_HEADER, "" + sessionKey);

            if (requestType != HttpRequestType.GET) {
                httpCon.setDoOutput(true);
                httpCon.addRequestProperty("Content-Length", "" + requestBody.length());
                switch (requestType) {
                    case PUT:
                        httpCon.setRequestMethod(HttpPut.METHOD_NAME);
                        break;
                    case POST:
                        httpCon.setRequestMethod(HttpPost.METHOD_NAME);
                        break;
                }

                //sending request to the server.
                OutputStream outputStream = httpCon.getOutputStream();
                Writer writer = new OutputStreamWriter(outputStream);
                writer.write(requestBody);
                writer.flush();
                writer.close();
            } else
                httpCon.setRequestMethod(HttpGet.METHOD_NAME);

            //getting the response from the server
            InputStream inputStream = httpCon.getInputStream();
            BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(inputStream));
            ByteArrayBuffer byteArrayBuffer = new ByteArrayBuffer(50);

            int intResponse;

            while ((intResponse = bufferedReader.read()) != -1) {
                byteArrayBuffer.append(intResponse);
            }

            responseStorage = new String(byteArrayBuffer.toByteArray());

        } catch (Exception aException) {
            responseStorage = aException.getMessage();
            this.transactionSucceeded = false;
        }

        return responseStorage;
    }

    public void getRequest(String url, String sessionKey) {
        requestAddress = url;
        this.sessionKey = sessionKey;
        requestType = HttpRequestType.GET;
    }

    public void putRequest(String url, String json, String sessionKey) {
        requestAddress = url;
        this.sessionKey = sessionKey;
        requestType = HttpRequestType.PUT;
        requestBody = json;
    }

    public void postRequest(String url, String json, String sessionKey) {
        requestAddress = url;
        this.sessionKey = sessionKey;
        requestType = HttpRequestType.POST;
        requestBody = json;
    }

    @Override
    protected void onPostExecute(String result) {
        if (transactionSucceeded) {
            result = trimString(result);
            callBack.onResult(result);
        } else {
            error.onFail();
        }
    }

    private String trimString(String result) {
        if (result == null || result.length() == 0) {
            return "";
        }

        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 1, len = result.length() - 1; i < len; i++) {
            stringBuilder.append(result.charAt(i));
        }

        return stringBuilder.toString();
    }
}