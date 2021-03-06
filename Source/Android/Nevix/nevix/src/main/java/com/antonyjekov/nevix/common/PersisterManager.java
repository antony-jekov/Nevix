package com.antonyjekov.nevix.common;

import com.antonyjekov.nevix.common.contracts.FailCallback;
import com.antonyjekov.nevix.viewmodels.UserLoginViewModel;
import com.google.gson.Gson;

import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Formatter;

public class PersisterManager {
    private String sessionKey;
    //private final String ROOT_ADDRESS = "http://nevix.apphb.com/api/";
    private final String ROOT_ADDRESS = "http://nevix-remote.com/api/";
    public static final String SESSION_KEY_HEADER = "X-SessionKey";

    public PersisterManager() {
        this(ContextManager.EMPTY_SESSION_KEY);
    }

    public PersisterManager(String sessionKey) {
        this.sessionKey = sessionKey;
    }

    public boolean isUserLoggedIn() {
        return !sessionKey.equals(ContextManager.EMPTY_SESSION_KEY);
    }

    public void wakeUpInternet() {
        HttpAsyncRequest request = new HttpAsyncRequest(new HttpAsyncRequest.OnResultCallBack() {
            @Override
            public void onResult(String result) {

            }
        }, new FailCallback() {
            @Override
            public void onFail() {

            }
        });
        request.getRequest("http://google.com", "");

        request.execute();
    }

    public void login(String email, String pass, HttpAsyncRequest.OnResultCallBack callBack, FailCallback error) {
        UserLoginViewModel model = new UserLoginViewModel(email.toLowerCase(), stringToSha1(pass));
        String json = new Gson().toJson(model, UserLoginViewModel.class);
        HttpAsyncRequest request = new HttpAsyncRequest(callBack, error);
        request.putRequest(ROOT_ADDRESS + "user/login", json, null);
        request.execute();
    }

    public void getLastMediaUpdateTime(HttpAsyncRequest.OnResultCallBack callback, FailCallback error) {
        HttpAsyncRequest request = new HttpAsyncRequest(callback, error);
        request.getRequest(ROOT_ADDRESS + "User/LastMediaUpdate", sessionKey);

        request.execute();
    }

    private String stringToSha1(String password) {
        String sha1 = "";
        try {
            MessageDigest crypt = MessageDigest.getInstance("SHA-1");
            crypt.reset();
            crypt.update(password.getBytes("UTF-8"));
            sha1 = byteToHex(crypt.digest());
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }

        return sha1;
    }

    private String byteToHex(final byte[] hash) {
        Formatter formatter = new Formatter();
        for (byte b : hash) {
            formatter.format("%02x", b);
        }

        String result = formatter.toString();
        formatter.close();

        return result;
    }

    public void setSessionKey(String sessionKey) {
        this.sessionKey = sessionKey;
    }

    public void getMedia(HttpAsyncRequest.OnResultCallBack callback, FailCallback error) {
        HttpAsyncRequest request = new HttpAsyncRequest(callback, error);
        request.getRequest(ROOT_ADDRESS + "mediafoldersmobile/getfolders", sessionKey);

        request.execute();
    }

    public void resetPass(String emailText, HttpAsyncRequest.OnResultCallBack onResult, FailCallback onFail) {
        HttpAsyncRequest request = new HttpAsyncRequest(onResult, onFail);
        request.putRequest(ROOT_ADDRESS + "user/forgottenpassword?email=" + emailText, "", "");

        request.execute();
    }
}
