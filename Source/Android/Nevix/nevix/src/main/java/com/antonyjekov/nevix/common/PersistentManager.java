package com.antonyjekov.nevix.common;

import com.antonyjekov.nevix.common.contracts.IAsyncResponse;
import com.antonyjekov.nevix.viewmodels.UserLoginViewModel;
import com.antonyjekov.nevix.viewmodels.UserRegisterViewModel;
import com.google.gson.Gson;

import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Formatter;

public class PersistentManager {
    private String sessionKey;
    private final String ROOT_ADDRESS = "http://nevix.apphb.com/api/";
    public static final String SESSION_KEY_HEADER = "X-SessionKey";

    private IAsyncResponse delegate;

    public PersistentManager(IAsyncResponse delegate) {
        this(delegate, null);
    }

    public PersistentManager(IAsyncResponse delegate, String sessionKey) {
        this.sessionKey = sessionKey;
        this.delegate = delegate;
    }

    public boolean isUserLoggedIn() {
        return sessionKey != null;
    }

    public void login(String email, String pass) {
        UserLoginViewModel model = new UserLoginViewModel(email.toLowerCase(), stringToSha1(pass));
        String json = new Gson().toJson(model, UserLoginViewModel.class);
        HttpAsyncRequest request = new HttpAsyncRequest(delegate);
        request.putRequest(ROOT_ADDRESS + "user/login", json, null);

        request.execute();
    }

    public void register(String email, String password, String confirm) {
        UserRegisterViewModel model = new UserRegisterViewModel(email, stringToSha1(password), stringToSha1(confirm));
        String json = new Gson().toJson(model, UserRegisterViewModel.class);
        HttpAsyncRequest request = new HttpAsyncRequest(delegate);
        request.postRequest(ROOT_ADDRESS + "user/register", json, null);

        request.execute();
    }

    public void getChannelName() {
        HttpAsyncRequest request = new HttpAsyncRequest(delegate);
        request.getRequest(ROOT_ADDRESS + "user/getchannel", sessionKey);

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
}
