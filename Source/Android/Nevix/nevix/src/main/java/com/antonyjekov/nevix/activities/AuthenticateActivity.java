package com.antonyjekov.nevix.activities;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.ActionBarActivity;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.HttpAsyncRequest;
import com.antonyjekov.nevix.common.PersistentManager;
import com.antonyjekov.nevix.fragments.AuthorizationFragment;

public class AuthenticateActivity extends ActionBarActivity {

    private PersistentManager persistant;
    public static final String SESSION_KEY_EXTRA = "com.antonyjekov.nevix.auth.sessionKey";
    public static final String CHANNEL_NAME_EXTRA = "com.antonyjekov.nevix.auth.channelName";
    private ContextManager data;
    private String channelName;
    private String sessionKey;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_authenticate);

        data = new ContextManager(this);
        String sessionKey = data.getSessionKey();

        if (sessionKey != null && !sessionKey.equals(ContextManager.EMPTY_SESSION_KEY)) {

        }

        if (savedInstanceState == null) {
            this.persistant = new PersistentManager();
            getSupportFragmentManager().beginTransaction()
                    .add(R.id.container_auth, new AuthorizationFragment(new HttpAsyncRequest.OnResultCallBack() {
                        @Override
                        public void onResult(String result) {
                            handleLogin(result);
                        }
                    }))
                    .commit();
        }
    }

    private void handleLogin(String result) {
        this.sessionKey = result;
        data.setSessionKey(result);
        this.persistant.setSessionKey(result);
        getChanelName();
    }

    private void getChanelName() {
        HttpAsyncRequest request = new HttpAsyncRequest(new HttpAsyncRequest.OnResultCallBack() {
            @Override
            public void onResult(String result) {
                channelName = result;
                finishLogin();
            }
        });
    }

    private void finishLogin() {
        Intent intent = new Intent(this, MainActivity.class);
        intent.putExtra(SESSION_KEY_EXTRA, this.sessionKey);
        intent.putExtra(CHANNEL_NAME_EXTRA, this.channelName);

        startActivity(intent);
        finish();
    }
}