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
    private ContextManager data;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_authenticate);

        data = new ContextManager(this);
        String sessionKey = data.getSessionKey();
        if (sessionKey != ContextManager.EMPTY_SESSION_KEY) {

        }

        if (savedInstanceState == null) {
            this.persistant = new PersistentManager();
            getSupportFragmentManager().beginTransaction()
                    .add(R.id.container_auth, new AuthorizationFragment(new HttpAsyncRequest.OnResultCallBack() {
                        @Override
                        public void onResult(String result) {
                            // TODO handle registration/login
                        }
                    }))
                    .commit();
        }
    }
}