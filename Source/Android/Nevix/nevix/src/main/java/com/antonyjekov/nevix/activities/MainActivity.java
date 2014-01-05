package com.antonyjekov.nevix.activities;

import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuInflater;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.PersistentManager;
import com.antonyjekov.nevix.common.contracts.IAsyncResponse;

public class MainActivity extends ActionBarActivity implements IAsyncResponse {

    PersistentManager persistent;
    ContextManager data;
    private final String NEVIX_DATA = "NevixData";

    public static final int AUTH_REQUEST_CODE = 1;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        String sessionKey = getSessionKey();
        if (sessionKey == null || sessionKey.length() == 0) {
            Intent login = new Intent(this, AuthenticateActivity.class);
            startActivityForResult(login, AUTH_REQUEST_CODE);
        } else {
            this.persistent = new PersistentManager(this, sessionKey);
            Test(sessionKey);
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu items for use in the action bar
        MenuInflater inflater = getMenuInflater();
        inflater.inflate(R.menu.main, menu);

        return super.onCreateOptionsMenu(menu);
    }

    private String getSessionKey() {
        SharedPreferences dataStorage = getSharedPreferences(NEVIX_DATA, 0);
        String sessionKey = dataStorage.getString("sessionKey", "");

        return sessionKey;
    }

    private void storeSessionKey(String sessionKey) {
        SharedPreferences.Editor dataStorageEditor = getSharedPreferences(NEVIX_DATA, 0).edit();
        dataStorageEditor.putString("sessionKey", sessionKey);

        dataStorageEditor.commit();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        switch (requestCode) {
            case AUTH_REQUEST_CODE:
                String sessionKey = data.getStringExtra(AuthenticateActivity.SESSION_KEY_EXTRA);
                Test(sessionKey);
                handleAuthorization(sessionKey);
                break;
        }
    }

    private void handleAuthorization(String sessionKey) {
        this.persistent = new PersistentManager(this, sessionKey);
        storeSessionKey(sessionKey);
    }

    @Override
    public void processFinish(String result) {

    }

    private void Test(String text) {
        Toast.makeText(this, text, Toast.LENGTH_LONG).show();
    }
}