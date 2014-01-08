package com.antonyjekov.nevix.common;

import android.content.Context;
import android.content.SharedPreferences;

public class ContextManager {

    SharedPreferences localData;
    SharedPreferences.Editor localEditor;

    private static final String NEVIX_DATA = "NevixData";
    public static final String EMPTY_SESSION_KEY = "";
    private static final String SESSION_KEY = "SessionKey";
    private static final String MEDIA_DATA = "MediaData";

    public ContextManager (Context context) {
        this.localData = context.getSharedPreferences(NEVIX_DATA, 0);
        this.localEditor = localData.edit();
    }

    public void setSessionKey(String sessionKey) {
        localEditor.putString(SESSION_KEY, sessionKey);

        localEditor.commit();
    }

    public String getSessionKey() {
        return localData.getString(SESSION_KEY, EMPTY_SESSION_KEY);
    }

    public void storeMediaDatabase(String data) {
        localEditor.putString(MEDIA_DATA, data);
    }

    public String getMediaDatabase() {
        return localData.getString(MEDIA_DATA, "");
    }
}