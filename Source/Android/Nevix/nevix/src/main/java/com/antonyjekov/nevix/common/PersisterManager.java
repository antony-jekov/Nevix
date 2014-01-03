package com.antonyjekov.nevix.common;

import com.antonyjekov.nevix.models.UserLoginModel;
import com.google.gson.Gson;

import org.apache.http.HttpResponse;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpPut;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Formatter;

public class PersisterManager {
    private String sessionKey;
    private final String ROOT_ADDRESS = "http://10.0.2.2:50906/api/";
    private final String SESSION_KEY_HEADER = "X-SessionKey";

    public PersisterManager() {
    }

    public PersisterManager(String sessionKey) {
        this.sessionKey = sessionKey;
    }

    public boolean isUserLoggedIn() {
        return sessionKey != null;
    }

    public void login(String email, String pass) {
        UserLoginModel model = new UserLoginModel(email.toLowerCase(), stringToSha1(pass));
        String json = new Gson().toJson(model, UserLoginModel.class);

        HttpResponse result = makePutRequest(ROOT_ADDRESS + "user/login", json);
    }

    private HttpResponse makeGetRequest(String uri) {
        try {
            HttpGet httpGet = new HttpGet(uri);
            httpGet.setHeader("Accept", "application/json");
            //httpPost.setHeader("Content-type", "application/json");
            httpGet.setHeader(SESSION_KEY_HEADER, sessionKey);
            return new DefaultHttpClient().execute(httpGet);
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        } catch (ClientProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }

        return null;
    }

    private HttpResponse makePostRequest(String uri, String json) {
        try {
            HttpPost httpPost = new HttpPost(uri);
            httpPost.setEntity(new StringEntity(json));
            httpPost.setHeader("Accept", "application/json");
            httpPost.setHeader("Content-type", "application/json");
            httpPost.setHeader(SESSION_KEY_HEADER, sessionKey);
            return new DefaultHttpClient().execute(httpPost);
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        } catch (ClientProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }

        return null;
    }

    private HttpResponse makePutRequest(String uri, String json) {
        try {
            HttpPut httpPut = new HttpPut(uri);
            httpPut.setEntity(new StringEntity(json));
            httpPut.setHeader("Accept", "application/json");
            httpPut.setHeader("Content-type", "application/json");
            httpPut.setHeader(SESSION_KEY_HEADER, sessionKey);
            return new DefaultHttpClient().execute(httpPut);
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        } catch (ClientProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }

        return null;
    }

    private String stringToSha1(String password)
    {
        String sha1 = "";
        try
        {
            MessageDigest crypt = MessageDigest.getInstance("SHA-1");
            crypt.reset();
            crypt.update(password.getBytes("UTF-8"));
            sha1 = byteToHex(crypt.digest());
        }
        catch(NoSuchAlgorithmException e)
        {
            e.printStackTrace();
        }
        catch(UnsupportedEncodingException e)
        {
            e.printStackTrace();
        }
        return sha1;
    }

    private String byteToHex(final byte[] hash)
    {
        Formatter formatter = new Formatter();
        for (byte b : hash)
        {
            formatter.format("%02x", b);
        }
        String result = formatter.toString();
        formatter.close();
        return result;
    }
}
