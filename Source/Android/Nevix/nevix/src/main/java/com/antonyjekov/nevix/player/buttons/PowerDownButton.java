package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 3/25/2014.
 */
public class PowerDownButton extends FullScreenButton {

    public PowerDownButton(Rect button, int strokeWidth, int padding) {
        super(button, strokeWidth, padding);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        shape.moveTo(button.centerX(), (float) (button.centerY() - ((button.height() >> 2) + padding * .3)));
        shape.lineTo(button.centerX(), button.centerY());
    }

    @Override
    public String command() {
        return PlayerCommand.POWER_CMD;
    }
}
