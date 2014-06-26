
%-------------------------------------------------------------------------------

addpath('D:/projects/emophiz/tools/matlab');

root = 'D:/projects/emophiz/data/logs/experiment/adaptation_values';

%-------------------------------------------------------------------------------

%1 time_millisecond
%2 arousal
%3 player_speed
%4 zombie_speed
%5 fog_start_dist
%6 fog_end_dist
%7 current_round
%8 zombie_threshold
%9 zombie_increase_power
%10 max_zombie_alive
%11 number_of_alive_zombies
%12 number_of_killed_zombies
%13 grenade_regen_delay
%14 medic_regen_delay
%15 calibrating
%16 adaptation_condition

%-------------------------------------------------------------------------------

file_id = {
    10
    12
    13
    14
    15
    16
    17
    18
    19
    20
    21
    22
    23
    24
    25
    26
};

phase_offset = 500000;
signal_offset = 100;
means = zeros(26, 4);
variances = zeros(26, 4);
ranges = zeros(26, 4);
killed_zombies = zeros(26, 4);
total_killed_zombies = zeros(26, 4);
max_round = zeros(26, 4);
adaptation_value = zeros(26, 4);
time_duration = zeros(26, 4);

positive_adaptation_integral = zeros(26, 4);
negative_adaptation_integral = zeros(26, 4);
positive_adaptation_kills = zeros(26, 4);
negative_adaptation_kills = zeros(26, 4);
positive_over_negative_time_proportion = zeros(26, 4);

for j=1:4
    mn = 0;
    for i=1:length(file_id)
        gsr_filename            = sprintf('%s/%d_%s_%d.csv', root, file_id{i}, 'gsr', j);
        metrics_filename        = sprintf('%s/%d_%s_%d.csv', root, file_id{i}, 'metrics', j);

        %gsr_filename            = sprintf('%s/%d_%s.csv', root, file_id{i}, 'gsr');
        %metrics_filename        = sprintf('%s/%d_%s.csv', root, file_id{i}, 'metrics');

        [gsr_h, gsr_v]          = readCSV(gsr_filename, 3);
        [metrics_h, metrics_v]  = readCSV(metrics_filename, 16);

        gsr_signal_clamped = gsr_v{2} - gsr_v{2}(1);
        gsr_time_adjusted = gsr_v{1} - gsr_v{1}(1) + phase_offset * j;

        means(file_id{i}, j) = mean(gsr_signal_clamped); %* 1000;
        variances(file_id{i}, j) = var(gsr_signal_clamped);
        ranges(file_id{i}, j) = (max(gsr_signal_clamped) - min(gsr_signal_clamped)); % * 1000;
        time_duration(file_id{i}, j) = (gsr_v{1}(end) - gsr_v{1}(1)) / 60000.0;
        killed_zombies(file_id{i}, j) = max(metrics_v{12}) / ((gsr_v{1}(end) - gsr_v{1}(1)) / 60000.0);
        total_killed_zombies(file_id{i}, j) = max(metrics_v{12});
        max_round(file_id{i}, j) = max(metrics_v{7});

        if j == 1
            adaptation_value(file_id{i}, j) = 0;
            positive_adaptation_integral(file_id{i}, j) = 0;
            negative_adaptation_integral(file_id{i}, j) = 0;
            positive_over_negative_time_proportion(file_id{i}, j) = 0;
            positive_adaptation_kills(file_id{i}, j) = 0;
            negative_adaptation_kills(file_id{i}, j) = 0;
        else
            adaptation = 0;
            if j == 2
                adaptation = metrics_v{j+1} - 1.325;
            elseif j == 3
                adaptation = metrics_v{j+1} - 1.25;
            elseif j == 4
                adaptation = metrics_v{j+1} - 260;
            end
            
            adaptation_value(file_id{i}, j) = mean(adaptation);

            % positive adaptation integral
            p = adaptation; %metrics_v{2} - 0.5;
            p(p < 0) = 0;
            positive_adaptation_integral(file_id{i}, j) = trapz(metrics_v{1}, p);

            % negative adaptation integral
            n = adaptation; %metrics_v{2} - 0.5;
            n(n > 0) = 0;
            negative_adaptation_integral(file_id{i}, j) = trapz(metrics_v{1}, n);

            % positive/negative adaptation time proportion
            p(p > 0) = 1;
            n(n < 0) = 1;
            p_i = trapz(metrics_v{1}, p);
            n_i = trapz(metrics_v{1}, n);
            positive_over_negative_time_proportion(file_id{i}, j) = p_i / n_i;

            % number of kills per pos/neg adaptation
            z_killed = metrics_v{12};
            for k = numel(z_killed):-1:2
                if z_killed(k) > 0
                    z_killed(k) = z_killed(k) - z_killed(k-1);
                end
            end
            %zombie_killed_arr(file_id{i}, j) = z_killed;

            % number of kills in positive adaptations
            z_killed_p = 0;
            for k = 1:numel(p)
                if (p(k) > 0)
                    z_killed_p = z_killed_p + z_killed(k);
                end
            end
            positive_adaptation_kills(file_id{i}, j) = z_killed_p;

            %dbstop if (j == 2 && field_id{i} == 15);
                
            % number of kills in negative adaptations
            z_killed_n = 0;
            for k = 1:numel(n)
                if (n(k) > 0)
                    z_killed_n = z_killed_n + z_killed(k);
                end
            end
            negative_adaptation_kills(file_id{i}, j) = z_killed_n;
        end

        %plot(gsr_time_adjusted, smooth(gsr_v{1}, gsr_signal_clamped * 1000 + signal_offset * i, 0.1, 'loess'), 'g-'); hold on;

        %plot(gsr_v{1}, gsr_v{2} * 1000 + 700, 'b-'); hold on; % gsr
        %plot(metrics_v{1}, metrics_v{15} * 100 + 25, 'k-'); hold on; % calibration
        %plot(metrics_v{1}, metrics_v{16} * 20 + 50, 'c-'); hold on; % adaptation
        %plot(metrics_v{1}, (metrics_v{3}) * 100 / 2.3, 'y-'); %hold on; % player speed
        %plot(metrics_v{1}, (metrics_v{4} - 1 ) * 100 / 3, 'g-'); %hold on; % zombie speed
        %plot(metrics_v{1}, (metrics_v{5} - 70) * 100 / 380, 'm-'); %hold on; % fog distant
        %legend('GSR Data', 'Calibration', 'Condition', 'Player Speed', 'Zombie Speed', 'Fog Distant', 'Location','NW')

        xlabel('time');
        ylabel('signal');
        %grid on;
    end

end

bar(means, 0.4);
bar(ranges, 0.2);


%-------------------------------------------------------------------------------
