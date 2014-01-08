package com.antonyjekov.nevix.activities;

import android.os.Bundle;
import android.support.v7.app.ActionBarActivity;
import android.view.Menu;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.PersistentManager;
import com.antonyjekov.nevix.common.PusherManager;
import com.antonyjekov.nevix.common.contracts.IAsyncResponse;
import com.antonyjekov.nevix.constants.PlayerCommand;

public class MainActivity extends ActionBarActivity implements IAsyncResponse {

    private PersistentManager persistent;
    private ContextManager data;
    private PusherManager pusher;

    private Button playBtn;
    private Button pauseBtn;
    private Button nextBtn;
    private Button prevBtn;
    private Button volumeUpBtn;
    private Button sysVolumeUpBtn;
    private Button volumeDownBtn;
    private Button sysVolumeDownBtn;
    private Button rwBtn;
    private Button ffBtn;
    private Button fullBtn;
    private Button stepFBtn;
    private Button stepBBtn;
    private Button powerBtn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        String sessionKey = getIntent().getStringExtra(AuthenticateActivity.SESSION_KEY_EXTRA);

        if (sessionKey == null || sessionKey.equals("") || sessionKey.length() == 0) {
            throw new IllegalArgumentException("invalid session key.");
        }

        this.persistent = new PersistentManager(this, sessionKey);
        persistent.getChannelName();

        data = new ContextManager(this);

        playBtn = (Button) findViewById(R.id.play_btn);
        pauseBtn = (Button) findViewById(R.id.pause_btn);
        nextBtn = (Button) findViewById(R.id.next_btn);
        prevBtn = (Button) findViewById(R.id.prev_btn);
        volumeUpBtn = (Button) findViewById(R.id.volume_up_btn);
        volumeDownBtn = (Button) findViewById(R.id.volume_down_btn);
        rwBtn = (Button) findViewById(R.id.rw_btn);
        ffBtn = (Button) findViewById(R.id.ff_btn);
        fullBtn = (Button) findViewById(R.id.full_btn);
        stepBBtn = (Button) findViewById(R.id.step_b_btn);
        stepFBtn = (Button) findViewById(R.id.step_f_btn);
        sysVolumeDownBtn = (Button) findViewById(R.id.sys_volume_down_btn);
        sysVolumeUpBtn = (Button) findViewById(R.id.sys_volume_up_btn);
        powerBtn = (Button) findViewById(R.id.power_btn);

        playBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.PLAY_CMD);
            }
        });

        pauseBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.PAUSE_CMD);
            }
        });

        nextBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.NEXT_CMD);
            }
        });

        prevBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.PREV_CMD);
            }
        });

        volumeUpBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.VOLUME_UP_CMD);
            }
        });

        volumeDownBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.VOLUME_DOWN_CMD);
            }
        });

        rwBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.RW_CMD);
            }
        });

        ffBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.FF_CMD);
            }
        });

        fullBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.FULL_CMD);
            }
        });

        stepFBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.STEP_F_CMD);
            }
        });

        stepBBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.STEP_B_CMD);
            }
        });

        sysVolumeUpBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.SYS_VOLUME_UP_CMD);
            }
        });

        sysVolumeDownBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.SYS_VOLUME_DOWN_CMD);
            }
        });

        powerBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                pusher.pushCommand(PlayerCommand.POWER_CMD);
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.test, menu);
        return true;
    }

    @Override
    public void processFinish(String result) {
        pusher = new PusherManager(result);
        test("Connected to channel: " + result);
    }

    private void test(String text) {
        Toast.makeText(this, text, Toast.LENGTH_LONG).show();
    }
}