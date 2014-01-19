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
    private static final String LAST_UPDATE = "LastUpdate";

    public ContextManager (Context context) {
        this.localData = context.getSharedPreferences(NEVIX_DATA, 0);
        this.localEditor = localData.edit();
    }

    public void setSessionKey(String sessionKey) {
        storeData(SESSION_KEY, sessionKey);
    }

    public String getSessionKey() {
        return localData.getString(SESSION_KEY, EMPTY_SESSION_KEY);
    }

    public void storeMediaDatabase(String data) {
        storeData(MEDIA_DATA, data);
    }

    public String getMediaDatabase() {
        return localData.getString(MEDIA_DATA, "");
    }

    public String getLastDatabaseUpdate() {
        return localData.getString(LAST_UPDATE, "");
    }

    public void setLastDatabaseUpdate(String time) {
        storeData(LAST_UPDATE, time);
    }

    private void storeData (String key, String data) {
        localEditor.putString(key, data);

        localEditor.commit();
    }
}