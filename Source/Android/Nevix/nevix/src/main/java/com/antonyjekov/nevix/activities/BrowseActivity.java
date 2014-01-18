package com.antonyjekov.nevix.activities;

import android.support.v7.app.ActionBarActivity;
import android.support.v7.app.ActionBar;
import android.support.v4.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.os.Build;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.HttpAsyncRequest;
import com.antonyjekov.nevix.common.PersistentManager;

public class BrowseActivity extends ActionBarActivity {

    ContextManager db;
    PersistentManager persistentManager;

    public static final String BROWSED_FILE = "com.antonyjekov.nevix.browse.browsedFile";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_browse);

        db = new ContextManager(this);
        String sessionKey = db.getSessionKey();
        persistentManager = new PersistentManager(sessionKey);

        final String lastLocalUpdate = db.getLastDatabaseUpdate();
        persistentManager.getLastMediaUpdateTime(new HttpAsyncRequest.OnResultCallBack() {
            @Override
            public void onResult(String result) {
                if (result == null || !result.equals(lastLocalUpdate) ) {
                    pringMessage(getResources().getString(R.string.media_is_outdated));
                }
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.browse, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        syncMedia();
        return true;
    }

    private void syncMedia() {
        pringMessage(getResources().getString(R.string.beginning_media_sync));

        persistentManager.getMedia(new HttpAsyncRequest.OnResultCallBack() {
            @Override
            public void onResult(String result) {
                db.storeMediaDatabase(result);
                // TODO: Update UI.
            }
        });
    }

    private void pringMessage(String message) {
        Toast.makeText(this, message, Toast.LENGTH_LONG).show();
    }
}