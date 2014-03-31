package com.antonyjekov.nevix.player;

import android.content.Context;
import android.graphics.Rect;

import com.antonyjekov.nevix.activities.MainActivity;
import com.antonyjekov.nevix.common.PusherManager;
import com.antonyjekov.nevix.player.buttons.BrowseButton;
import com.antonyjekov.nevix.player.buttons.FastForwardButton;
import com.antonyjekov.nevix.player.buttons.FullScreenButton;
import com.antonyjekov.nevix.player.buttons.MuteButton;
import com.antonyjekov.nevix.player.buttons.NextButton;
import com.antonyjekov.nevix.player.buttons.PauseButton;
import com.antonyjekov.nevix.player.buttons.PlayButton;
import com.antonyjekov.nevix.player.buttons.PowerDownButton;
import com.antonyjekov.nevix.player.buttons.PreviousButton;
import com.antonyjekov.nevix.player.buttons.RewindButton;
import com.antonyjekov.nevix.player.buttons.StopButton;
import com.antonyjekov.nevix.player.buttons.SystemVolumeDownButton;
import com.antonyjekov.nevix.player.buttons.SystemVolumeUpButton;
import com.antonyjekov.nevix.player.buttons.VolumeDownButton;
import com.antonyjekov.nevix.player.buttons.VolumeUpButton;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public class BSPlayer extends Player {

    public BSPlayer(Context context, PusherManager pusher, MainActivity owner) {
        super(context, pusher, owner);
    }

    @Override
    protected void arrangeButtons() {
        int halfWidth = width >> 1;
        int halfHeight = height >> 1;

        int buttonHalfSize = ((int) (width * .15)) >> 1;
        Rect playBtn = new Rect(halfWidth - buttonHalfSize, halfHeight - buttonHalfSize, halfWidth + buttonHalfSize, halfHeight + buttonHalfSize);
        buttons.add(new PlayButton(playBtn));

        int buttonSize = ((int) (width * .12));

        Rect ffBtn = rightTo(playBtn, buttonSize);
        buttons.add(new FastForwardButton(ffBtn));

        Rect nextBtn = rightTo(ffBtn, buttonSize);
        buttons.add(new NextButton(nextBtn));

        Rect rwBtn = leftTo(playBtn, buttonSize);
        buttons.add(new RewindButton(rwBtn));

        Rect prevBtn = leftTo(rwBtn, buttonSize);
        buttons.add(new PreviousButton(prevBtn));

        Rect pauseBtn = onTopOf(playBtn, buttonSize);
        buttons.add(new PauseButton(pauseBtn));

        Rect stopBtn = bellowOf(playBtn, buttonSize);
        buttons.add(new StopButton(stopBtn));

        buttonSize = (int) (width * .08);

        Rect powerBtn = new Rect(buttonMargin, buttonMargin, buttonMargin + buttonSize, buttonMargin + buttonSize);
        buttons.add(new PowerDownButton(powerBtn));

        Rect fullBtn = new Rect(width - (buttonSize + buttonMargin), buttonMargin, width - buttonMargin, buttonMargin + buttonSize);
        buttons.add(new FullScreenButton(fullBtn));

        Rect volUpBtn = bellowOf(fullBtn, buttonSize);
        buttons.add(new VolumeUpButton(volUpBtn));

        Rect volumeDownBtn = bellowOf(volUpBtn, buttonSize);
        buttons.add(new VolumeDownButton(volumeDownBtn));

        Rect sysVolUp = bellowOf(powerBtn, buttonSize);//new Rect(buttonMargin, buttonMargin, buttonMargin + buttonSize, buttonMargin + buttonSize);
        buttons.add(new SystemVolumeUpButton(sysVolUp));

        Rect sysVolDown = bellowOf(sysVolUp, buttonSize);
        buttons.add(new SystemVolumeDownButton(sysVolDown));

        Rect browseBtn = leftTo(fullBtn, buttonSize);
        buttons.add(new BrowseButton(browseBtn));

        Rect muteBtn = rightTo(powerBtn, buttonSize);
        buttons.add(new MuteButton(muteBtn));
    }
}
