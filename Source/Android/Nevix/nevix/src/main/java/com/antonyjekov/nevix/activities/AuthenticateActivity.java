package com.antonyjekov.nevix.activities;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.ActionBarActivity;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.PersistentManager;
import com.antonyjekov.nevix.common.contracts.IAsyncResponse;
import com.antonyjekov.nevix.fragments.AuthorizationFragment;

public class AuthenticateActivity extends ActionBarActivity implements IAsyncResponse {

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
            this.persistant = new PersistentManager(this);
            getSupportFragmentManager().beginTransaction()
                    .add(R.id.container_auth, new AuthorizationFragment(persistant))
                    .commit();
        }
    }

    @Override
    public void processFinish(String result) {
        Intent intent = new Intent();
        intent.putExtra(SESSION_KEY_EXTRA, result);

        setResult(RESULT_OK, intent);
        finish();
    }
}