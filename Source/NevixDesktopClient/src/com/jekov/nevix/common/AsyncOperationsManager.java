package com.jekov.nevix.common;

import java.util.concurrent.*;

/**
 * Created by Antony Jekov on 3/28/2014.
 */
public class AsyncOperationsManager {
    private ExecutorService executorService = Executors.newFixedThreadPool(3);

    public static final String GET = "GET";
    public static final String PUT = "PUT";
    public static final String POST = "POST";

    public void performHttpGet(String address, String sessionKey, CallBack callBack) {
        HttpAsyncRequest request = new HttpAsyncRequest(callBack);
        request.prepareCall(address, GET, sessionKey, "");
        this.executorService.submit(request);
    }

    public void performHttpPost(String url, String sessionKey, String requestBody, CallBack callBack) {
        HttpAsyncRequest request = new HttpAsyncRequest(callBack);
        request.prepareCall(url, POST, sessionKey, requestBody);
        this.executorService.submit(request);
    }

    public void performHttpPut(String url, String sessionKey, String requestBody, CallBack callBack) {
        HttpAsyncRequest request = new HttpAsyncRequest(callBack);
        request.prepareCall(url, PUT, sessionKey, requestBody);
    }
}
