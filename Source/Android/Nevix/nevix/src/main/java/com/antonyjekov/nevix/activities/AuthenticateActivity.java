package com.antonyjekov.nevix.activities;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.ActionBarActivity;
import android.widget.Toast;

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

    Context thisContext;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_authenticate);
        thisContext = this;
        data = new ContextManager(this);

        if (savedInstanceState == null) {
            this.persistant = new PersistentManager();

            getSupportFragmentManager().beginTransaction()
                    .add(R.id.container_auth, new AuthorizationFragment(new HttpAsyncRequest.OnResultCallBack() {
                        @Override
                        public void onResult(String result) {
                            handleLogin(result);
                            data.setSessionKey(result);
                        }
                    }))
                    .commit();
        }
    }

    @Override
    protected void onResume() {
        super.onResume();

        String sessionKey = data.getSessionKey();

        if (sessionKey != null && !sessionKey.equals(ContextManager.EMPTY_SESSION_KEY)) {
            handleLogin(sessionKey);
        }
    }

    private void handleLogin(String result) {
        this.sessionKey = result;
        data.setSessionKey(result);
        this.persistant.setSessionKey(result);
        getChanelName();
    }

    private void getChanelName() {
        HttpAsyncRequest.OnResultCallBack callBack = new HttpAsyncRequest.OnResultCallBack() {
            @Override
            public void onResult(String result) {

                if (channelName != null && channelName.length() > 0) {
                    channelName = result;
                    finishLogin();
                } else {
                    Toast.makeText(thisContext, "No channel!\nMake sure you have started the Nevix Desktop Client.", Toast.LENGTH_LONG);
                }

            }
        };

        persistant.getChannelName(callBack);
    }

    private void finishLogin() {
        Intent intent = new Intent(this, MainActivity.class);
        intent.putExtra(SESSION_KEY_EXTRA, this.sessionKey);
        intent.putExtra(CHANNEL_NAME_EXTRA, this.channelName);

        startActivity(intent);
        finish();
    }
}