package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public class FullScreenButton extends SphericalButton {
    public FullScreenButton(Rect button) {
        super(button);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        shape.addCircle(button.centerX(), button.centerY(), button.height() >> 2, Path.Direction.CW);
    }

    @Override
    public String command() {
        return PlayerCommand.FULL_CMD;
    }
}
